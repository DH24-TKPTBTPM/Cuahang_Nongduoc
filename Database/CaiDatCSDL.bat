@echo off
REM Tao CSDL CuahangNongduoc tren SQL Server va nap du lieu tu Access.
REM Can: SQL Server Express + sqlcmd. Doi ten instance o dong duoi neu khac.
REM -C: tin chung chi tu ky cua SQL Server tren may (ODBC Driver 18 mac dinh bat ma hoa).
set SERVER=.\SQLEXPRESS
cd /d "%~dp0"
for %%f in (01_CreateDatabase.sql 02_CreateTables.sql 03_SeedData.sql 04_KiemTraDuLieu.sql 06_Migration_NguoiDung_GiaVon.sql) do (
  echo === %%f ===
  sqlcmd -S %SERVER% -E -C -b -f 65001 -i %%f || goto :loi
)
echo.
echo XONG. CSDL CuahangNongduoc tren SQL Server da duoc tao va cap nhat day du!
pause
exit /b 0
:loi
echo.
echo CO LOI - xem thong bao o tren.
pause
exit /b 1
