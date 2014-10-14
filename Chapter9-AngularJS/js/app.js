var app = angular.module('calcApp',[]);

app.controller('CalcController', function(){
	this.currentNumber = "";
	this.currentTotal = "0";
	this.displayThis = "0";
	this.mathAction = "";
	this.previousMathAction = "";
	this.previousNumber = "";
	
	this.CreateNumber = function(digit){
		this.currentNumber += digit;
		this.displayThis = this.currentNumber;
	};
	
	this.SetMathAction = function(symbol){
		if(this.mathAction !== ""){
			this.Process();
		}
		else if(this.currentNumber !== ""){
			this.currentTotal = this.currentNumber;
		}
		
		this.currentNumber = "";
		this.mathAction = symbol;
	};
	
	this.Equals = function(){
		if(this.currentNumber === ""){
			this.mathAction = this.previousMathAction;
			this.currentNumber = this.previousNumber;
		}
		this.Process();
	}
	
	this.Process = function(){
		if(this.mathAction === "" || this.currentNumber === "") return;
		
		if(this.mathAction === '+'){
			this.currentTotal = parseFloat(this.currentTotal) + parseFloat(this.currentNumber);
		}
		else if(this.mathAction === '-'){
			this.currentTotal = parseFloat(this.currentTotal) - parseFloat(this.currentNumber);
		}
		else if(this.mathAction === '*'){
			this.currentTotal = parseFloat(this.currentTotal) * parseFloat(this.currentNumber);
		}
		else if(this.mathAction === '/'){
			this.currentTotal = parseFloat(this.currentTotal) / parseFloat(this.currentNumber);
		}
		
		this.previousMathAction = this.mathAction;
		this.mathAction = "";
		this.previousNumber = this.currentNumber;
		this.currentNumber = "";
		
		this.displayThis = this.currentTotal;
	};
	
	this.ClearAll = function(){
		this.currentTotal = "0";
		this.currentNumber = "";
		this.displayThis = "0";
		this.mathAction = "";
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