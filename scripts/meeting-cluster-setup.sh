#!/bin/bash

mongo rs.initiate({
    _id : 'meeting-cluster',
    members: [
        {'_id': 0, 'host': 'meeting_db_rs0:27017' },
        {'_id': 1, 'host': 'meeting_db_rs1:27017' },
        {'_id': 2, 'host': 'meeting_db_rs2:27017' }
    ]
})