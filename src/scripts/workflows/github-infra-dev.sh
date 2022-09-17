#!/bin/bash

# Default github action is at ./src working_dir

# run docker compose
docker-compose -f docker-compose-infra.dev.yml -p wizbooking-infra build
