'use strict';

var Sequelize = require('sequelize');

// Actions summary:
// createTable "post", deps: []

var info = {
    "revision": 1,
    "name": "post",
    "created": "2020-01-17T06:34:53.402Z",
    "comment": ""
};

var migrationCommands = [{
    fn: "createTable",
    params: [
        "post",
        {
            "postId": {
                "allowNull": false,
                "autoIncrement": true,
                "primaryKey": true,
                "field": "PostId",
                "seqType": "Sequelize.INTEGER"
            },
            "userId": {
                "allowNull": false,
                "primaryKey": true,
                "field": "UserId",
                "seqType": "Sequelize.INTEGER"
            },
            "message": {
                "allowNull": false,
                "seqType": "Sequelize.STRING",
                "autoIncrement": true,
                "primaryKey": true,
                "field": "Message"
            },
            "isDraft": {
                "defaultValue": true,
                "value": true,
                "field": "isDraft",
                "seqType": "Sequelize.BOOLEAN"
            },
            "deleted": {
                "seqType": Sequelize.BOOLEAN,
                "field": "Deleted",
                "defaultValue": false
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
    {}
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
