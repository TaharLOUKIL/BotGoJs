Un éditeur graphique pour la conception de chatbots

C'est la partie Back-End qui nous servira à la persistance des diagrammes dans une base de données NoSql MongoDB.

1. Partie Back-End sous forme de web API développé en C# .NET Core :
Cet API est développé dans l'objectif d'assurer la persistance des diagrammes dans une base de données NoSql MongoDB.

La base de données est accessible avec l'outil MongoDB Compass (https://www.mongodb.com/products/compass) via la chaine de donnexion suivante : 
mongodb+srv://gojs:aBOCMgLAum8vTecv@cluster0.nlkyi.mongodb.net/?retryWrites=true&w=majority

	-	Après avoir cloner la solution de l'API (https://github.com/TaharLOUKIL/BotGoJs.git), il faut la lancer sur Visual Studio 2022.
	-	Postman nous permet de tester d'une façon unitaire les différentes méthodes implémentées


2. Partie Front-End sous forme de projet Angular/Html-Css : assure la création graphique des diagrammess de chatbots.
   Fait appel à l'API  pour la persistance dans la base de données.
   
	-	Cloner la solution du projet Anglar (https://github.com/TaharLOUKIL/modern-admin-angular.git).
	-	Lancer un terminal depuis le dossier du projet
	-	Exécuter la commande "Code ." dans le terminal pour l'édition du projet avec visual studio code
	-	Exécuter la commande  "npm install @angular/cli@latest" (une seule fois au début)
	-	Exécuter la commende  "npm run mserve" (à chaque fois on veut lancer l'application)

