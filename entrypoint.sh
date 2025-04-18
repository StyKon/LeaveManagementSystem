#!/bin/bash

echo "Running EF Core migrations..."
dotnet ef database update \
    --project LeaveManagementSystem.DATA \
    --startup-project LeaveManagementSystem.WebApi \
    --configuration Release

echo "Starting the app..."
dotnet LeaveManagementSystem.WebApi.dll
