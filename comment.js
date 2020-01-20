'use strict';
module.exports = (sequelize, DataTypes) => {
	var comment = sequelize.define(
		'comment',
		{
			CommentId: {
				allowNull: false,
				autoIncrement: true,
				primaryKey: true,
				type: DataTypes.INTEGER
		},
			Comment: {
				allowNull: false,
                type: DataTypes.STRING,
                isDraft: true,
                primaryKey: true
        },
            isDraft: DataTypes.BOOLEAN,
            UserId: {
			   type: DataTypes.INTEGER,
			references: {
				model: 'users',
                key: 'UserId'
        },
			createdAt: {
				type: DataTypes.DATE,
				allowNull: false
		},
			updatedAt: {
				type: DataTypes.DATE,
				allowNull: false
		}
    },
});

	return comment;
};
