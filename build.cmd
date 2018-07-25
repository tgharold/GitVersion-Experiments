@echo off
gitversion
powershell ./build.ps1 -Verbosity Diagnostic
