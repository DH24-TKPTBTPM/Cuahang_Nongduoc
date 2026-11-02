# Xuat schema + du lieu tu file Access cuahang.dll ra JSON de sinh script SQL Server.
# Chay bang PowerShell 32-bit (Jet OLEDB 4.0 chi co ban 32-bit):
#   C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe -ExecutionPolicy Bypass -File Database\tools\export_access.ps1
$ErrorActionPreference = "Stop"
$root = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)
$mdb  = Join-Path $root "Cuahang_Nongduoc\Cuahang Nongduoc\bin\Debug\cuahang.dll"
$out  = Join-Path $PSScriptRoot "access_dump.json"
$cn = New-Object System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=$mdb;")
$cn.Open()
function T2L($t){ $l=@(); foreach($r in $t.Rows){ $o=[ordered]@{}; foreach($c in $t.Columns){ $v=$r[$c]; if($v -is [DBNull]){$v=$null} elseif($v -is [DateTime]){$v=$v.ToString("yyyy-MM-ddTHH:mm:ss")} elseif($v -is [byte[]]){$v=[Convert]::ToBase64String($v)}; $o[$c.ColumnName]=$v }; $l+= ,$o }; return ,$l }
$res=[ordered]@{}
$tables = $cn.GetOleDbSchemaTable([System.Data.OleDb.OleDbSchemaGuid]::Tables, @($null,$null,$null,"TABLE"))
$res.tables = T2L $tables
$res.columns = T2L ($cn.GetOleDbSchemaTable([System.Data.OleDb.OleDbSchemaGuid]::Columns, $null))
$res.pks = T2L ($cn.GetOleDbSchemaTable([System.Data.OleDb.OleDbSchemaGuid]::Primary_Keys, $null))
$res.fks = T2L ($cn.GetOleDbSchemaTable([System.Data.OleDb.OleDbSchemaGuid]::Foreign_Keys, $null))
$res.indexes = T2L ($cn.GetOleDbSchemaTable([System.Data.OleDb.OleDbSchemaGuid]::Indexes, $null))
$data=[ordered]@{}
foreach($t in $tables.Rows){ $n=$t["TABLE_NAME"]; $da=New-Object System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [$n]",$cn); $dt=New-Object System.Data.DataTable; [void]$da.Fill($dt); $data[$n]= T2L $dt; Write-Host "$n : $($dt.Rows.Count) dong" }
$res.data=$data
$cn.Close()
$json = $res | ConvertTo-Json -Depth 6 -Compress
[System.IO.File]::WriteAllText($out, $json, (New-Object System.Text.UTF8Encoding($false)))
Write-Host "Da ghi $out"
# Kiem tra SQL Server tren may
Write-Host "--- SQL Server ---"
Get-Service | Where-Object { $_.Name -like 'MSSQL*' } | Format-Table Name,Status -AutoSize | Out-String | Write-Host
$s = Get-Command sqlcmd -ErrorAction SilentlyContinue; if($s){ Write-Host "sqlcmd: $($s.Source)" } else { Write-Host "sqlcmd: khong co" }
