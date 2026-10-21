param(
    [Parameter(Mandatory = $true)][string]$ConnectionString,
    [switch]$Apply
)
$ErrorActionPreference = 'Stop'
# Stop old clients and back up the database before running with -Apply.
# Default mode only counts rows and never logs credentials or changes data.
Add-Type -AssemblyName System.Data
if (-not ('ProjectSecurity.PasswordSecurity' -as [type])) {
    Add-Type -Path (Join-Path $PSScriptRoot '../Poligraf/Security/PasswordSecurity.cs')
}
$connection = New-Object System.Data.SqlClient.SqlConnection($ConnectionString)
$connection.Open()
$transaction = $null
try {
    if ($Apply) { $transaction = $connection.BeginTransaction([System.Data.IsolationLevel]::Serializable) }
    $command = $connection.CreateCommand()
    if ($transaction) { $command.Transaction = $transaction }
    $command.CommandText = 'SELECT [Id], [Password] FROM [dbo].[Employee] WHERE [Password] IS NOT NULL AND LEN([Password]) > 0'
    $rows = New-Object System.Collections.Generic.List[object]
    $reader = $command.ExecuteReader()
    while ($reader.Read()) {
        $rows.Add(@{ Id = $reader.GetValue(0); Stored = $reader.GetString(1) })
    }
    $reader.Close()
    $legacy = @($rows | Where-Object { -not [ProjectSecurity.PasswordSecurity]::IsHash($_.Stored) })
    if (-not $Apply) {
        Write-Output ('Legacy password rows requiring migration: ' + $legacy.Count)
        return
    }
    $command.CommandText = "SELECT is_nullable FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Employee') AND name = 'Password'"
    $nullable = [bool]$command.ExecuteScalar()
    $nullability = if ($nullable) { 'NULL' } else { 'NOT NULL' }
    $command.CommandText = 'ALTER TABLE [dbo].[Employee] ALTER COLUMN [Password] nvarchar(256) ' + $nullability
    [void]$command.ExecuteNonQuery()
    foreach ($row in $legacy) {
        if ($row.Stored.StartsWith('pbkdf2-sha256$')) { throw 'Malformed hash: reset the affected account before migration.' }
        $command.Parameters.Clear()
        $command.CommandText = 'UPDATE [dbo].[Employee] SET [Password] = @hash WHERE [Id] = @id'
        [void]$command.Parameters.Add('@hash', [System.Data.SqlDbType]::NVarChar, 256)
        $command.Parameters['@hash'].Value = [ProjectSecurity.PasswordSecurity]::Hash($row.Stored)
        [void]$command.Parameters.AddWithValue('@id', $row.Id)
        [void]$command.ExecuteNonQuery()
    }
    $transaction.Commit()
    Write-Output ('Password rows migrated: ' + $legacy.Count)
} catch {
    if ($transaction) { $transaction.Rollback() }
    throw
} finally {
    $connection.Dispose()
}
