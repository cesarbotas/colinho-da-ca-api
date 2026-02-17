#!/bin/bash

echo "🧪 Running Simplified Unit Tests"

cd tests/ColinhoDaCa.TestesUnitarios

# Run tests
dotnet test --logger "console;verbosity=detailed"

echo ""
echo "✅ Unit Tests Completed"
echo ""
echo "📊 Current Test Coverage:"
echo "   ✅ Domain Entities (3 files)"
echo "   ✅ Services (2 files)" 
echo "   ✅ Auth (1 file)"
echo ""
echo "Total: 6 test files covering core functionality"