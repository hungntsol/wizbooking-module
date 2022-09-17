#!/bin/bash

# move to src folder
cd ../

# run docker compose
docker-compose -f docker-compose-infra.dev.yml -p wizbooking-infra build
