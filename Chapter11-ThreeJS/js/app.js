var app = angular.module('viewerApp',[]);

app.controller('ViewerController', function(){
	this.Hey = function(){
		console.log("HEEEY");
	}
	
	this.Switch = function(){
		ClearScene();
		loadSphere = !loadSphere;
		if(loadSphere){
			MakeSphere();
		}
		else{
			Load();
		}
	}
});