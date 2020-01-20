'use strict';
module.exports = (sequelize, DataTypes) => {
	const post = sequelize.define(
		'post',
		{
			postId: {
				allowNull: false,
                primaryKey: true,
                autoIncrement: true,
				type: DataTypes.INTEGER
		},
			isDraft: DataTypes.BOOLEAN,
	        Message: DataTypes.STRING,
		    UserId: {
			   type: DataTypes.INTEGER,
			references: {
				model: 'users',
                key: 'UserId'
        },
			Deleted: {
				type: DataTypes.BOOLEAN,
				defaultValue: false
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
    
post.associate = function(models) {
		// associations can be defined here
	};
	return post;
};
