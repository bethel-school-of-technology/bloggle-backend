'use strict';

var Sequelize = require('sequelize');

/**
 * Actions summary:
 *
 * createTable "users", deps: []
 *
 **/

var info = {
    "revision": 1,
    "name": "users",
    "created": "2020-01-17T06:34:53.402Z",
    "comment": ""
};

var migrationCommands = [{
    fn: "createTable",
    params: [
        "users",
        {
            "userId": {
                "allowNull": false,
                "autoIncrement": true,
                "primaryKey": true,
                "field": "UserId",
                "seqType": "Sequelize.INTEGER"
            },
            "username": {
                "unique": true,
                "field": "Username",
                "seqType": "Sequelize.STRING"
            },
            "password": {
                "field": "Password",
                "seqType": "Sequelize.STRING"
            },
            "email": {
                "unique": true,
                "field": "Email",
                "seqType": "Sequelize.STRING"
            },
            "firstName": {
                "field": "FirstName",
                "seqType": "Sequelize.STRING"
            },
            "lastName": {
                "field": "LastName",
                "seqType": "Sequelize.STRING"
            },
            "location": {
                "field": "Location",
                "seqType": "Sequelize.STRING"
            },
            "admin": {
                "defaultValue": false, 
                "allowNull": false,
                "field": "Admin",
                "seqType": "Sequelize.BOOLEAN"
            },
            "deleted": {
                "defaultValue": false,
                "field": "Deleted",
                "seqType": "Sequelize.BOOLEAN"
            },
            "createdAt": {
                "allowNull": false,
                "field": "createdAt",
                "seqType": "Sequelize.DATE"
            },
            "updatedAt": {
                "allowNull": false,
                "field": "updatedAt",
                "seqType": "Sequelize.DATE"
            }
        },
    ]
}];

module.exports = {
    pos: 0,
    up: function(queryInterface, Sequelize)
    {
        var index = this.pos;
        return new Promise(function(resolve, reject) {
            function next() {
                if (index < migrationCommands.length)
                {
                    let command = migrationCommands[index];
                    console.log("[#"+index+"] execute: " + command.fn);
                    index++;
                    queryInterface[command.fn].apply(queryInterface, command.params).then(next, reject);
                }
                else
                    resolve();
            }
            next();
        });
    },
    info: info
};
