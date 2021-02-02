$path = Get-Location

$baseDate=[datetime]"09/11/2016"
$currentDate=$(Get-Date)
$interval=New-TimeSpan -Start $baseDate -End $currentDate
$days=$interval.Days
$hours=$interval.Hours

foreach($line in Get-Content .\projects.txt) {
    $projectName= "..\$line\$line.csproj"
    Write-Host $projectName
    if (Test-Path "..\$line\bin")
    {
        rm -Recurse -Force "..\$line\bin"
    }
    if (Test-Path "..\$line\obj")
    {
        rm -Recurse -Force "..\$line\obj"
    }
    dotnet pack --configuration Release --output "..\$line\bin\nupkgs" $projectName /p:Version="0.1.0-alpha2-$days"

    dotnet nuget push "..\$line\bin\nupkgs\*.nupkg"  -s https://api.nuget.org/v3/index.json

    rm -Recurse -Force "..\$line\bin"
    rm -Recurse -Force "..\$line\obj"
}

