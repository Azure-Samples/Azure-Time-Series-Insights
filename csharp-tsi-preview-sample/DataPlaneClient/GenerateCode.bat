
@echo ===============
@echo GENERATING CODE
@echo ===============

@echo on
AutoRest .\azure-rest-api-specs\specification\timeseriesinsights\data-plane\readme.md --csharp-sdks-folder=.\Generated --csharp --verbose --latest --azure-validator --model-validator --semantic-validator
@echo off

@echo.
@echo DONE.
@pause
