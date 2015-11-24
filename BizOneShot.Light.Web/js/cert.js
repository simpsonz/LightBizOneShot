function checkCertificate(userNo) {
    //return true;
    // 모든 Condition 설정.
    var nRet = TSToolkit.SetConfig("test", CA_LDAP_INFO, CTL_INFO, POLICIES,
                            INC_CERT_SIGN, INC_SIGN_TIME_SIGN, INC_CRL_SIGN, INC_CONTENT_SIGN,
                            USING_CRL_CHECK, USING_ARL_CHECK);
    if (nRet > 0) {
        alert(nRet + " : " + TSToolkit.GetErrorMessage());
        return false;
        //return true; //테스트때문에 임시로 true리턴
    }

    // 인증서 로딩시 KM 인증서 검증 옵션 켜기
    TSToolkit.SetEachConfig("LOAD_CERT_KM", "TRUE");

    // 사용자가 자신의 인증서를 선택. 
    nRet = TSToolkit.SelectCertificate(STORAGE_TYPE, 0, "");
    if (nRet > 0) {
        alert(nRet + " : " + TSToolkit.GetErrorMessage());
        return false;
        //return true; //테스트때문에 임시로 true리턴
    }
    nRet = TSToolkit.VerifyVID(userNo);
    if (nRet > 0) {
        alert(nRet + " : " + TSToolkit.GetErrorMessage());
        return false;
        //return true; //테스트때문에 임시로 true리턴
    }

    if (TSToolkit.OutData != "true") {
        alert("사업자번호가 일치하지 않습니다.");
        return false;
        //return true; //테스트때문에 임시로 true리턴
    }
    return true;

}