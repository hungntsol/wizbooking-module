#!/bin/bash

# move to src
cd ../src

# run compose
docker-compose -f ./docker-compose-infra.dev.yml -p wizbooking-infra down
docker-compose -f ./docker-compose-infra.dev.yml -p wizbooking-infra up -d
