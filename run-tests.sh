#!/bin/bash

echo "🚀 Starting Colinho da Cá Test Suite"

# Build and start services
echo "📦 Building and starting services..."
docker-compose -f docker-compose.test.yml up -d --build

# Wait for services to be ready
echo "⏳ Waiting for services to be ready..."
sleep 30

# Run unit tests with coverage
echo "🧪 Running unit tests with coverage..."
cd tests/ColinhoDaCa.TestesUnitarios
dotnet test --collect:"XPlat Code Coverage" --results-directory ./TestResults

# Generate coverage report
echo "📊 Generating coverage report..."
dotnet tool install -g dotnet-reportgenerator-globaltool 2>/dev/null || true
reportgenerator -reports:"./TestResults/*/coverage.cobertura.xml" -targetdir:"./TestResults/CoverageReport" -reporttypes:Html

cd ../..

# Run integration tests
echo "🔗 Running integration tests..."
cd tests/ColinhoDaCa.TestesIntegrados
dotnet test --logger "console;verbosity=detailed"

cd ../..

# Display results
echo "✅ Test suite completed!"
echo "📊 Coverage report: tests/ColinhoDaCa.TestesUnitarios/TestResults/CoverageReport/index.html"
echo "🌐 API running at: http://localhost:5000"

# Keep services running for manual testing
echo "🔧 Services are still running for manual testing. Press Ctrl+C to stop."
docker-compose -f docker-compose.test.yml logs -f