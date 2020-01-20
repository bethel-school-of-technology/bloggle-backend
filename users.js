'use strict';
module.exports = (sequelize, DataTypes) => {
	var users = sequelize.define(
		'users',
		{
			UserId: {
				allowNull: false,
				autoIncrement: true,
				primaryKey: true,
				type: DataTypes.INTEGER
			},
			Username: {
				type: DataTypes.STRING,
				unique: true
			},
			FirstName: DataTypes.STRING,
			LastName: DataTypes.STRING,
			Email: {
				type: DataTypes.STRING,
				unique: true
			},			
			Password: DataTypes.STRING,
			Admin: {
				type: DataTypes.BOOLEAN,
				defaultValue: false,
				allowNull: false
            },
            Location: DataTypes.STRING,
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
		{}
	);

	return users;
};
