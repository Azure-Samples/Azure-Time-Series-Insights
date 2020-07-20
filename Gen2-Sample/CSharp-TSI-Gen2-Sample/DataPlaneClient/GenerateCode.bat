
@echo ===============
@echo GENERATING CODE
@echo ===============

@echo on
AutoRest readme.md --csharp-sdks-folder=.\Generated --csharp --verbose --latest --azure-validator --model-validator --semantic-validator
@echo off

@echo.
@echo DONE.
@pause
