var app = angular.module('calcApp',[]);

app.controller('CalcController', function(){
	this.Answer = "";
	this.Question = "";
	
	this.Parrot = function(sentence){
		this.Answer = sentence;
	};
});