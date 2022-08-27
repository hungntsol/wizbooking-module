title 'Wizbooking Infastructure'

cd ../

docker-compose -f docker-compose-infra.dev.yml down
docker-compose -f docker-compose-infra.dev.yml up -d

pause