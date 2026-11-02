@echo off
REM Tao CSDL CuahangNongduoc tren SQL Server va nap du lieu tu Access.
REM Can: SQL Server Express + sqlcmd. Doi ten instance o dong duoi neu khac.
set SERVER=.\SQLEXPRESS
cd /d "%~dp0"
for %%f in (01_CreateDatabase.sql 02_CreateTables.sql 03_SeedData.sql 04_KiemTraDuLieu.sql) do (
  echo === %%f ===
  sqlcmd -S %SERVER% -E -b -f 65001 -i %%f || goto :loi
)
echo.
echo XONG. Xem bang doi chieu o tren: tat ca phai la "Khop".
pause
exit /b 0
:loi
echo.
echo CO LOI - xem thong bao o tren.
pause
exit /b 1
