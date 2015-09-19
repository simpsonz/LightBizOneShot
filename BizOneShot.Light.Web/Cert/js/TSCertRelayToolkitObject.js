
var OSName="Unknown OS";
if (navigator.appVersion.indexOf("Win")!=-1) OSName="Windows";
if (navigator.appVersion.indexOf("Mac")!=-1) OSName="MacOS";
if (navigator.appVersion.indexOf("X11")!=-1) OSName="UNIX";
if (navigator.appVersion.indexOf("Linux")!=-1) OSName="Linux";

var attributes = { id:'TSCertRelayToolkitReal',code:'tradesign.certrelay.applet.MainApplet"', width:100, height:100} ;
var parameters = {jnlp_href: '../download/certrelay.jnlp'} ;
//deployJava.runApplet(attributes, parameters, '1.6');

//alert(OSName);
if(OSName=="Linux" || OSName=="UNIX")
{
//	alert(OSName);
//	alert(deployJava.getJREs());
	var jreversion = deployJava.getJREs();
//	alert("hi2");
//	alert(jreversion.length);
//	alert(jreversion.indexOf("1.7"));
	if((jreversion.length<1) || (jreversion.indexOf("1.7")!=-1))
	 {
		 alert(" Java 런타임 환경이 설치되지 않아 Java 환경 설치로 이동합니다.");
	 	location.replace("http://www.webupd8.org/2012/01/install-oracle-java-jdk-7-in-ubuntu-via.html");
	 	
	 } else
	 {
	 		deployJava.runApplet(attributes, parameters, '1.6');
	 }
	
}

if(OSName=="Windows" )
{
	var jreversion = deployJava.getJREs();
	
	Array.prototype.indexOf = function(object) { 
		for (var i = 0, length = this.length; i < length; i++) 
			if (this[i] == object) return i; 
		return -1; 
	}; 
	
	if( jreversion.length<1 || (jreversion.indexOf("1.7") != -1)) 
	 {
	 	alert(" Java 런타임 환경이 설치되지 않아 Java 환경 설치로 이동합니다.");
	 }
	 
	 deployJava.runApplet(attributes, parameters, '1.6');
}

if(OSName=="MacOS")
{
	deployJava.runApplet(attributes, parameters, '1.6');
}

var callbackFunction;
var callBackArgs;

function RegisterCallback(cbFunction)
{
  var callback = cbFunction.toString(); // We get JS code
  var callbackName = /^function (\w+)\(/.exec(callback);
  TSCertRelayToolkitReal.RegisterCallback(callbackName[1]);
}



function callBackAtSelectCertificate(a)
{
//	alert("callback " + a );
	isSelectCertificateShowing = false;
	nRet = TSCertRelayToolkitReal.GetResultSelectCertificate();
	if (nRet > 0)
	{
		alert(nRet + " : " + TSCertRelayToolkit.GetErrorMessage());
		return ;
	}
	if(callbackFunction!=null){
		TSCertRelayToolkit.OutData = TSCertRelayToolkitReal.OutData;
		callbackFunction(callBackArgs);
	}
	callbackFunction = null;
	callBackArgs=null;

}

function callBackAtGetCertCode(a)
{
	if(callbackFunction!=null){
		TSCertRelayToolkit.OutData = TSCertRelayToolkitReal.OutData;
		callbackFunction(callBackArgs);
	}
	callbackFunction = null;
	callBackArgs=null;
}

function TSCertRelayToolkitRealWrapper ()
{	
	var OutData;
	var OutDataNum;
	
}



TSCertRelayToolkitRealWrapper.prototype.init=function ()
{
		
		  //alert("initToolKit");
		 
		  TSCertRelayToolkitReal.initToolKit();
		  return 0;
}


TSCertRelayToolkitRealWrapper.prototype.loadKeyAndCert=function ( type , style, user,user2, func, args)
{
		
	 	   //alert("loadKeyAndCert" + args.length);
	
	 	   callbackFunction = func;
	 	   callBackArgs = args;
	 	   RegisterCallback(callBackAtSelectCertificate);
		   var nRet = TSCertRelayToolkitReal.loadKeyAndCert(type , style, user,user2);
		   return nRet ;
		
}
TSCertRelayToolkitRealWrapper.prototype.GetCertCode=function (func, args)
{
		
		  //alert("getCertCode");
		callbackFunction = func;
		callBackArgs = args;
		RegisterCallback(callBackAtGetCertCode);
		var nRet = TSCertRelayToolkitReal.GetCertCode();
		return nRet;
}
TSCertRelayToolkitRealWrapper.prototype.Net_SendPFX=function (strpfx, certcode, dn, func, args)
{
	
	//alert("Net_SendPFX");
	
	callbackFunction = func;
	callBackArgs = args;
	RegisterCallback(callBackAtGetCertCode);
	var nRet = TSCertRelayToolkitReal.Net_SendPFX(strpfx, certcode, dn);
	return nRet;
}

TSCertRelayToolkitRealWrapper.prototype.Net_RcvPFX=function (certcode, func, args)
{
	
	//alert("Net_RcvPFX");
	
	callbackFunction = func;
	callBackArgs = args;
	RegisterCallback(callBackAtGetCertCode);
	var nRet = TSCertRelayToolkitReal.Net_RcvPFX(certcode);
	return nRet;
}

TSCertRelayToolkitRealWrapper.prototype.makepfx=function (strsigncert, strsignpriv, strkmcert, strkmpriv, certpwd, pfxpwd)
{
	
	//alert("makepfx");
	
	var nRet = TSCertRelayToolkitReal.makepfx(strsigncert, strsignpriv, strkmcert, strkmpriv, certpwd, pfxpwd);
	this.OutData= TSCertRelayToolkitReal.OutData;
	return nRet;
}

TSCertRelayToolkitRealWrapper.prototype.extractpfx=function (strpfx, aftercertpwd, pfxpwd)
{
	
	//alert("extractpfx");
	
	var nRet = TSCertRelayToolkitReal.extractpfx(strpfx, aftercertpwd, pfxpwd);
	return nRet;
}

TSCertRelayToolkitRealWrapper.prototype.setUrl=function (url)
{
	
	//alert("setUrl");
	
	var nRet = TSCertRelayToolkitReal.setUrl(url);
	return nRet;
}

TSCertRelayToolkitRealWrapper.prototype.setGroupID=function (groupId)
{
	
	//alert("setGroupID");
	
	var nRet = TSCertRelayToolkitReal.setGroupID(groupId);
	return nRet;
}

TSCertRelayToolkitRealWrapper.prototype.setTimeOut=function (millisecond)
{
	
	//alert("setTimeOut");
	
	var nRet = TSCertRelayToolkitReal.setTimeOut(millisecond);
	return nRet;
}

TSCertRelayToolkitRealWrapper.prototype.GetCertificateDN=function ()
{
	
	//alert("GetCertificateDN");
	
	var nRet = TSCertRelayToolkitReal.GetCertificateDN();
	this.OutData= TSCertRelayToolkitReal.OutData;
	return nRet;
}

TSCertRelayToolkitRealWrapper.prototype.GetCertificate=function (certtype)
{
	
	//alert("getCertificate");
	
	var nRet = TSCertRelayToolkitReal.GetCertificate(certtype);
	this.OutData= TSCertRelayToolkitReal.OutData;
	return nRet;
}

TSCertRelayToolkitRealWrapper.prototype.GetPrivateKey=function (keytype)
{
	
	//alert("getPrivateKey");
	
	var nRet = TSCertRelayToolkitReal.GetPrivateKey(keytype);
	this.OutData= TSCertRelayToolkitReal.OutData;
	return nRet;
}

TSCertRelayToolkitRealWrapper.prototype.GetPrivateKey=function (keytype)
{
	
	//alert("getPrivateKey");
	
	var nRet = TSCertRelayToolkitReal.GetPrivateKey(keytype);
	this.OutData= TSCertRelayToolkitReal.OutData;
	return nRet;
}

TSCertRelayToolkitRealWrapper.prototype.getRandom=function (randomlength)
{
	
	//alert("getRandom");
	
	var nRet = TSCertRelayToolkitReal.getRandom(randomlength);
	this.OutData= TSCertRelayToolkitReal.OutData;
	return nRet;
}

TSCertRelayToolkitRealWrapper.prototype.GetErrorMessage=function ()
{
		
		 var nRet=  TSCertRelayToolkitReal.GetErrorMessage();
		 return nRet;
}

TSCertRelayToolkitRealWrapper.prototype.GetErrorCode=function ()
{
	
	var nRet=  TSCertRelayToolkitReal.GetErrorCode();
	return nRet;
}

TSCertRelayToolkitRealWrapper.prototype.InputPasswordDialog=function ()
{
	
	var nRet=  TSCertRelayToolkitReal.InputPasswordDialog();
	this.OutData= TSCertRelayToolkitReal.OutData;
	return nRet;
}


var TSCertRelayToolkit = new TSCertRelayToolkitRealWrapper();

