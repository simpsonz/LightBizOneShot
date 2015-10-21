
function getContextPath() {
	var offset = location.href.indexOf(location.host) + location.host.length;
	var ctxPath = location.href.substring(offset, location.href.indexOf('/',
			offset + 1));

	return ctxPath + '/cert';
	//return ""; // contextPath가 ROOT 일 경우
}
var attributes = {
	id : 'TSToolkitReal',
	code : 'com.ktnet.pki.multibrowser.pkitoolkit.applet.MainApplet',
	width : 50,
	height : 50,
	image: getContextPath() + '/images/loading.gif', //loading image
	centerimage : 'true', //loading image's
	boxbgcolor : 'white', //loading background color
	boxborder : 'false' //loading border
};
var parameters = {
	jnlp_href : getContextPath() + '/download/pkitoolkit.jnlp',
	separate_jvm : true,
	classloader_cache : false,
	backgroundImage: getContextPath() + '/images/white_bg.gif' //loading 후 image
	//backgroundImage: getContextPath() + '/images/checkIcon.jpg' //loading 후 image
	//,license: getContextPath() + '/download/localhost_LICENSE.dat'
};

runPKIMulti();

function matchStringInArray(actual, expected) {

	for (i in actual) {
		if (actual[i].match(expected) != null)
			return true;
	}
	return false;

}

function isJREInstalled() {
	var jreversion = deployJava.getJREs();
	
	if(jreversion.length < 1)
		return false;
	
	return matchStringInArray(jreversion, "1\.6.*|1\.7.*");
}

function runPKIMulti() {

	var OSName = "Unknown OS";
	if (navigator.appVersion.indexOf("Win") != -1)
		OSName = "Windows";
	if (navigator.appVersion.indexOf("Mac") != -1)
		OSName = "MacOS";
	if (navigator.appVersion.indexOf("X11") != -1)
		OSName = "UNIX";
	if (navigator.appVersion.indexOf("Linux") != -1)
		OSName = "Linux";
	// alert(OSName);
	if (OSName == "Linux" || OSName == "UNIX") {
		if (!isJREInstalled()) {
			alert(" Java 런타임 환경이 설치되지 않아 Java를 다운로드 설치 해야 합니다. 설치 후 재접속해주세요.");

			// 아래 url 중에 선택.
			location.replace("http://java.com/ko/download/manual.jsp"); // Download
																		// Page
			// // ubuntu install 방법
			// location.replace("http://www.webupd8.org/2012/01/install-oracle-java-jdk-7-in-ubuntu-via.html");
			// //rpm
			// location.replace("http://javadl.sun.com/webapps/download/AutoDL?BundleId=81809");
			// //tar
			// location.replace("http://javadl.sun.com/webapps/download/AutoDL?BundleId=81810");
			// //tar x64
			// location.replace("http://javadl.sun.com/webapps/download/AutoDL?BundleId=81812");
			// //rpm x64
			// location.replace("http://javadl.sun.com/webapps/download/AutoDL?BundleId=81811");
			//
			// location.replace("/jre/JavaSetup7-.tar"); //서버에 파일 올릴 경우.

		} else {
			deployJava.runApplet(attributes, parameters, '1.6');
		}

	}

	if (OSName == "Windows") {

		Array.prototype.indexOf = function(object) {
			for ( var i = 0, length = this.length; i < length; i++)
				if (this[i] == object)
					return i;
			return -1;
		};

		if (!isJREInstalled()) {
			alert(" Java 런타임 환경이 설치되지 않아 Java를 다운로드 합니다. 설치 후 재접속해주세요.");

			// 아래 url 중에 선택.
			location
					.replace("http://javadl.sun.com/webapps/download/AutoDL?BundleId=83394"); // 32bit
			// location.replace("http://javadl.sun.com/webapps/download/AutoDL?BundleId=81821"); //64bit
			// location.replace("http://java.com/ko/download/"); //Download Page
			// location.replace("/jre/JavaSetup7-.exe"); //서버에 파일 올릴 경우.

		} else {
			deployJava.runApplet(attributes, parameters, '1.6');
		}
	}

	if (OSName == "MacOS") {
		if (!isJREInstalled()) {
			alert(" Java 런타임 환경이 설치되지 않아 Java를 다운로드 합니다. 설치 후 재접속해주세요.");

			// 아래 url 중에 선택.
			location
					.replace("http://javadl.sun.com/webapps/download/AutoDL?BundleId=83377"); // mac
			// location.replace("http://www.java.com/en/download/mac_download.jsp");
			// location.replace("/jre/JavaSetup7-.pkg"); //웹서비스에 파일 올릴 경우.

		} else {
			deployJava.runApplet(attributes, parameters, '1.6');
		}
	}
}

var callbackFunction;
var callBackArgs;

function RegisterCallback(cbFunction) {
	var callback = cbFunction.toString(); // We get JS code
	var callbackName = /^function (\w+)\(/.exec(callback);
	TSToolkitReal.RegisterCallback(callbackName[1]);
}

function callBackAtSelectCertificate(a) {
	// alert("callback " + a );
	isSelectCertificateShowing = false;
	var nRet = TSToolkitReal.GetResultSelectCertificate();
	
	if (nRet > 0) {
		alert(nRet + " : " + TSToolkit.GetErrorMessage());
		return;
	}
	if (callbackFunction != null)
		callbackFunction(callBackArgs);
	callbackFunction = null;
	callBackArgs = null;

}

function TSToolkitRealWrapper() {
	var OutData;
	var OutDataNum;
}
TSToolkitRealWrapper.prototype.SelectCertificate = function(type, style, user,
		func, args) {

	var nRet;

	callbackFunction = func;
	callBackArgs = args;
	RegisterCallback(callBackAtSelectCertificate);
	nRet = TSToolkitReal.SelectCertificate(type, style, user);

	return nRet;

}

TSToolkitRealWrapper.prototype.SetConfig = function(name, ldap, info, policy,
		csign, stime, crl, csign, crl_check, arl_check) {
	var nRet = TSToolkitReal.SetConfig(name, ldap, info, policy, csign, stime,
			crl, csign, crl_check, arl_check);

	this.OutData = TSToolkitReal.OutData;
	return nRet;
}
TSToolkitRealWrapper.prototype.GetErrorMessage = function() {
	var nRet = TSToolkitReal.GetErrorMessage();
	return nRet;

}

TSToolkitRealWrapper.prototype.GetCertificate = function(cs, dt) {

	// alert("GetCertificate " + cs + " " + dt);
	var nRet = TSToolkitReal.GetCertificate(cs, dt);
	this.OutData = TSToolkitReal.OutData;
	return nRet;

}

TSToolkitRealWrapper.prototype.SignData = function(str) {

	// alert("SignData");

	var nRet = TSToolkitReal.SignData(str);
	this.OutData = TSToolkitReal.OutData;
	return nRet;

	return 0;
}

TSToolkitRealWrapper.prototype.GetCertificatePropertyFromID = function(c, v) {

	var nRet = TSToolkitReal.GetCertificatePropertyFromID(c, v);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.SetEachConfig = function(c, u) {

	var nRet = TSToolkitReal.SetEachConfig(c, u);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.VerifyVID = function(ssn) {
	// alert("VerifyVID");
	var nRet = TSToolkitReal.VerifyVID(ssn);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.VerifyVID2 = function(signcert, ssn, random) {

	var nRet = TSToolkitReal.VerifyVID2(signcert, ssn, random);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.GetRandomInKey = function() {

	var nRet = TSToolkitReal.GetRandomInKey();
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.DVCSService = function(serviceType, dvcsUrl,
		dataType, data, hashAlg) {

	var nRet = TSToolkitReal.DVCSService(serviceType, dvcsUrl, dataType, data,
			hashAlgo);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.SignFile = function(datapath, signfilepath) {

	var nRet = TSToolkitReal.SignFile(datapath, signfilepath);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.AddSignInSignedFile = function(signFilePath,
		addSignFilePath) {

	var nRet = TSToolkitReal.AddSignInSignedFile(signFilePath, addSignFilePath);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.VerifySignedFile = function(addSignFilePath,
		verifyFilePath, s) {

	var nRet = TSToolkitReal.VerifySignedFile(addSignFilePath, verifyFilePath,
			s);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.GetSignerCount = function() {

	var nRet = TSToolkitReal.GetSignerCount();
	this.OutDataNum = TSToolkitReal.OutDataNum;

	return 0;
}

TSToolkitRealWrapper.prototype.AddSignInSignedDataFile = function(s, o) {

	var nRet = TSToolkitReal.AddSignInSignedDataFile(s, o);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}
TSToolkitRealWrapper.prototype.AddSignInSignedData = function(s) {

	var nRet = TSToolkitReal.AddSignInSignedData(s);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}
TSToolkitRealWrapper.prototype.GetSignerDN = function(i) {

	var nRet = TSToolkitReal.GetSignerDN(i);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.GetSignerCert = function(i) {

	var nRet = TSToolkitReal.GetSignerCert(i);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}
TSToolkitRealWrapper.prototype.GetSigningTime = function(i) {

	var nRet = TSToolkitReal.GetSigningTime(i);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.CertificateValidation = function(c) {

	var nRet = TSToolkitReal.CertificateValidation(c);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.ClearMemory = function() {

	var nRet = TSToolkitReal.ClearMemory();
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}
TSToolkitRealWrapper.prototype.GetDataFromLDAP = function(dn, cert) {

	var nRet = TSToolkitReal.GetDataFromLDAP(dn, cert);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.Base64EncodeFile = function(filePath,
		base64EncodeFilePath) {

	var nRet = TSToolkitReal.Base64EncodeFile(filePath, base64EncodeFilePath);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.Base64DecodeFile = function(
		base64EncodeFilePath, base64DecodeFilePat) {

	var nRet = TSToolkitReal.Base64DecodeFile(base64EncodeFilePath,
			base64DecodeFilePat);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.SetEncryptionAlgoAndMode = function(id, mode) {

	var nRet = TSToolkitReal.SetEncryptionAlgoAndMode(id, mode);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}
TSToolkitRealWrapper.prototype.GetSymmetricIV = function() {

	var nRet = TSToolkitReal.GetSymmetricIV();
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.GetSymmetricKey = function() {

	var nRet = TSToolkitReal.GetSymmetricKey();
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.GetEncryptionMode = function() {

	var nRet = TSToolkitReal.GetEncryptionMode();
	this.OutDataNum = TSToolkitReal.OutDataNum;
	return nRet;
}

TSToolkitRealWrapper.prototype.GetEncryptionAlgorithm = function() {

	var nRet = TSToolkitReal.GetEncryptionAlgorithm();
	this.OutDataNum = TSToolkitReal.OutDataNum;
	return nRet;
}

TSToolkitRealWrapper.prototype.EncryptData = function(s, key, iv) {

	var nRet = TSToolkitReal.EncryptData(s, key, iv);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.DecryptData = function(s, key, iv) {

	var nRet = TSToolkitReal.DecryptData(s, key, iv);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.GenerateSymmetricKey = function(pwd) {

	var nRet = TSToolkitReal.GenerateSymmetricKey(pwd);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.EncryptFile = function(dataFilePath,
		encryptFilePath, secretKey, iv) {

	var nRet = TSToolkitReal.EncryptFile(dataFilePath, encryptFilePath,
			secretKey, iv);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.DecryptFile = function(encryptFilePath,
		decryptFilePath, secretKey, iv) {

	var nRet = TSToolkitReal.DecryptFile(encryptFilePath, decryptFilePath,
			secretKey, iv);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.SetPeerCertificate = function(pem) {

	var nRet = TSToolkitReal.SetPeerCertificate(pem);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.VerifySignedData = function(signedData, s) {

	var nRet = TSToolkitReal.VerifySignedData(signedData, s);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.Base64EncodeData = function(data) {

	var nRet = TSToolkitReal.Base64EncodeData(data);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.Base64DecodeData = function(data) {

	var nRet = TSToolkitReal.Base64DecodeData(data);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.HashData = function(nHashAlgorithmID, data) {
	var nRet = TSToolkitReal.HashData(nHashAlgorithmID, data);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.HashFile = function(nHashAlgorithmID,
		btDataFilePath) {

	var nRet = TSToolkitReal.HashFile(nHashAlgorithmID, btDataFilePath);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.GetPrivateKey = function(nKeyType,
		nKeyOutPutType, btPwd) {

	var nRet = TSToolkitReal.GetPrivateKey(nKeyType, nKeyOutPutType, btPwd);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.LoadCertificate = function(nKeyType, btSignCert,
		btSignKey, btKMCert, btKMKey, btPassword) {

	var nRet = TSToolkitReal.LoadCertificate(nKeyType, btSignCert, btSignKey,
			btKMCert, btKMKey, btPassword);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.GetVersion = function() {

	var nRet = TSToolkitReal.GetVersion();
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.LoginData = function(sessionID, ssn, userInfo) {
	var nRet = TSToolkitReal.LoginData(sessionID, ssn, userInfo);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.LoginDataProcess = function(loginData) {
	var nRet = TSToolkitReal.LoginDataProcess(loginData);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.GetLoginData = function(index) {
	var nRet = TSToolkitReal.GetLoginData(index);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.SetPolicyNameMap = function(policy) {
	var nRet = TSToolkitReal.SetPolicyNameMap(policy);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.AddPolicyNameMap = function(OID, Name) {
	var nRet = TSToolkitReal.AddPolicyNameMap(OID, Name);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.EnvelopData = function(plaintext) {
	var nRet = TSToolkitReal.EnvelopData(plaintext);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.DecryptEnvelopedData = function(envData) {
	var nRet = TSToolkitReal.DecryptEnvelopedData(envData);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.EnvelopFile = function(dataFilePath,
		envelopFilePath) {
	var nRet = TSToolkitReal.EnvelopFile(dataFilePath, envelopFilePath);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.DecryptEnvelopedFile = function(envelopFilePath,
		developFilePath) {
	var nRet = TSToolkitReal.DecryptEnvelopedFile(envelopFilePath,
			developFilePath);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}
TSToolkitRealWrapper.prototype.GenerateRandomNumber = function(nLength) {
	var nRet = TSToolkitReal.GenerateRandomNumber(nLength);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.GenSignature = function(dataType, plainText) {
	var nRet = TSToolkitReal.GenSignature(dataType, plainText);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.VerifySignature = function(dataType, plainText,
		signature, signerCert) {
	var nRet = TSToolkitReal.VerifySignature(dataType, plainText, signature,
			signerCert);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.GenerateRandomNumber = function(nLength) {
	var nRet = TSToolkitReal.GenerateRandomNumber(nLength);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}

TSToolkitRealWrapper.prototype.CertificateValidationByOCSP = function(certType,
		cert, ocspversion, hashAlgo, isSign) {
	var nRet = TSToolkitReal.CertificateValidationByOCSP(certType, cert,
			ocspversion, hashAlgo, isSign);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}
TSToolkitRealWrapper.prototype.SetStorageConfig = function(Respositype, enable) {
	var nRet = TSToolkitReal.SetStorageConfig(Respositype, enable);
	this.OutData = TSToolkitReal.OutData;
	return nRet;
}
TSToolkitRealWrapper.prototype.GetSelectedCertInfo = function(i) {
	var nRet = TSToolkitReal.GetSelectedCertInfo(i);
	this.OutData = TSToolkitReal.OutData;
	this.OutDataNum = TSToolkitReal.OutDataNum;
	return nRet;
}

var TSToolkit = new TSToolkitRealWrapper();