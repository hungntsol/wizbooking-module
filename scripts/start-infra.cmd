title 'wizbooking-infra'

cd ../src

docker-compose -f docker-compose-infra.dev.yml down
docker-compose -f docker-compose-infra.dev.yml up -d

@echo off
@echo wizbooking-infra compose up, auto close in 5s 

timeout 5
