#!/bin/bash

cd ./src
docker-compose -f docker-compose-infra.dev.yml -p wizbooking-infra build
