Feature: LobbyUsage
	Simple calculator for adding two numbers

@mytag
Scenario: A player creates a new lobby
	Given the username of the player TestUser
	Then TestUser creates a new lobby
	Then a new lobby will be created
	And a lobby code will be returned by the server to TestUser
	
Scenario: A player joins the lobby
	Given the username of the player LobbyJoiner
	Then LobbyJoiner joins the lobby
	Then a list of players is returned to LobbyJoiner

