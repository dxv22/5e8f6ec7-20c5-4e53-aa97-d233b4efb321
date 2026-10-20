FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY global.json Directory.Build.props .editorconfig LongestIncreasingSubsequence.sln ./
COPY src/LongestIncreasingSubsequence/*.csproj src/LongestIncreasingSubsequence/
COPY tests/LongestIncreasingSubsequence.Tests/*.csproj tests/LongestIncreasingSubsequence.Tests/
RUN dotnet restore

COPY src/ src/
COPY tests/ tests/

# Run the test suite with: docker build --target test .
FROM build AS test
RUN dotnet test --no-restore --configuration Release

FROM build AS publish
RUN dotnet publish src/LongestIncreasingSubsequence \
    --no-restore \
    --configuration Release \
    --output /app/publish

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER $APP_UID
ENTRYPOINT ["dotnet", "LongestIncreasingSubsequence.dll"]
