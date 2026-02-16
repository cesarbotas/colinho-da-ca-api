#!/bin/bash

# Create logs directory
mkdir -p logs

# Start logging stack
docker-compose -f docker-compose.logging.yml up -d

echo "Logging stack started:"
echo "- Grafana: http://localhost:3000 (admin/admin)"
echo "- Loki: http://localhost:3100"
echo "- Logs directory: ./logs"