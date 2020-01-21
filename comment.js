'use strict';

var Sequelize = require('sequelize');

// Actions summary:
// createTable "comment", deps: []


var info = {
    "revision": 1,
    "name": "comment",
    "created": "2020-01-17T09:47:32.913Z",
    "comment": ""
};

var migrationCommands = [{
    fn: "createTable",
    params: [
        "comment",
        {
            "commentId": {
                "type": Sequelize.INTEGER,
                "field": "CommentId",
                "primaryKey": true,
                "autoIncrement": true,
                "allowNull": false
            },
            "comment": {
                "allowNull": false,
                "seqType": "Sequelize.STRING",
                "autoIncrement": true,
                "primaryKey": true,
                "field": "Comment",
                "isDraft": "Sequelize.BOOLEAN"
            },
            "userId": {
                "allowNull": false,
                "primaryKey": true,
                "field": "UserId",
                "seqType": "Sequelize.INTEGER"
            },
            "isDraft": {
                    "defaultValue": true,
                    "value": true,
                    "field": "isDraft",
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
