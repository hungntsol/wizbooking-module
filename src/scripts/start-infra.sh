#!/bin/bash

# move to src
cd ../

# run compose
docker-compose -f ./docker-compose-infra.dev.yml -p wizbooking-infra down
docker-compose -f ./docker-compose-infra.dev.yml -p wizbooking-infra up -d
