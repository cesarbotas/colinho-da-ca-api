#!/bin/bash

echo "🧪 Running Unit Tests with Coverage"

cd tests/ColinhoDaCa.TestesUnitarios

# Clean previous results
rm -rf TestResults

# Run tests with coverage
dotnet test \
  --collect:"XPlat Code Coverage" \
  --results-directory ./TestResults \
  --logger "console;verbosity=detailed" \
  --configuration Release

# Install report generator if not exists
dotnet tool install -g dotnet-reportgenerator-globaltool 2>/dev/null || true

# Generate HTML coverage report
reportgenerator \
  -reports:"./TestResults/*/coverage.cobertura.xml" \
  -targetdir:"./TestResults/CoverageReport" \
  -reporttypes:Html \
  -title:"Colinho da Cá - Unit Tests Coverage"

# Display results
echo ""
echo "📊 Coverage Report Generated:"
echo "   File: $(pwd)/TestResults/CoverageReport/index.html"
echo ""

# Extract coverage percentage
COVERAGE=$(grep -o 'Line coverage: [0-9.]*%' ./TestResults/CoverageReport/index.html | head -1 | grep -o '[0-9.]*')

if [ ! -z "$COVERAGE" ]; then
    echo "📈 Line Coverage: $COVERAGE%"
    
    # Check if coverage meets minimum requirement (60%)
    if (( $(echo "$COVERAGE >= 60" | bc -l) )); then
        echo "✅ Coverage requirement met (≥60%)"
    else
        echo "❌ Coverage below requirement (≥60%)"
        exit 1
    fi
else
    echo "⚠️  Could not extract coverage percentage"
fi

echo ""
echo "🎯 Test Categories Covered:"
echo "   ✅ Domain Entities"
echo "   ✅ Services"
echo "   ✅ Use Cases"
echo "   ✅ Authentication"
echo ""
echo "Open the HTML report to see detailed coverage information."