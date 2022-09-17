title 'wizbooking-infra'

cd ../

docker-compose -f docker-compose-infra.dev.yml -p wizbooking-infra down
docker-compose -f docker-compose-infra.dev.yml -p wizbooking-infra up -d

@echo off
@echo wizbooking-infra compose up, auto close in 5s

timeout 5
