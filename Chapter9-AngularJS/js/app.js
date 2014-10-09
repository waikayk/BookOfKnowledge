var app = angular.module('calcApp',[]);

app.controller('CalcController', function(){
	this.Answer = "0";
	this.Question = "";
	
	this.Parrot = function(sentence){
		
		this.Answer = sentence;
	};
});

app.directive("calcButtons", function(){
	return{
		restrict: 'E',
		templateUrl: "html/calc-buttons.html",
		controller: "CalcController",
		controllerAs: "calc"
	};
});