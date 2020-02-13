@echo off
rmdir /q /s .vs
rem del   /q /s *.csproj.user
rmdir /q /s CommonLib\bin
rmdir /q /s Compiler\bin
rmdir /q /s LittleBattles\bin
rmdir /q /s OceanMaster\bin
rmdir /q /s LuaLib\bin
rmdir /q /s MovieMaker\bin
rmdir /q /s CommonLib\obj
rmdir /q /s Compiler\obj
rmdir /q /s MovieMaker\obj
rmdir /q /s LittleBattles\obj
rmdir /q /s OceanMaster\obj
rmdir /q /s LuaLib\obj

for /d %%i in (Teensy\Projects\*.*) do rmdir /q /s %%i\__vm
for /d %%i in (Teensy\Projects\*.*) do rmdir /q /s %%i\Release
for /d %%i in (Teensy\Projects\*.*) do rmdir /q /s %%i\Debug
