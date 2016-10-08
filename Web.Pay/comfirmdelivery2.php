<?php
require_once './lib/SnsNetwork.php';
require_once './lib/SnsSigCheck.php';
require_once './lib/SnsStat.php';
require_once 'OpenApiV3.php';
$appid = 1104965754;
$appkey = '4ftSvBSlY7BnYc59';
$method = 'GET';
$url_path = '/comfirmdelivery2.php';
//print_r($_SERVER['PHP_SELF'] . $_SERVER["QUERY_STRING"]);
$secret = '4ftSvBSlY7BnYc59&';
$parr;
parse_str($_SERVER["QUERY_STRING"],$parr);

foreach ($parr as $key => $val ) 
{ 
	if($val==null){		
		$parr[$key]="";
	}
}

/* $params = array(
		'amt' => $_GET['amt'],
		'appid' => $_GET['appid'],
		'billno' => $_GET['billno'],
		'kbazinga' => $_GET['kbazinga'],
		'openid' => $_GET['openid'],
		'payamt_coins' => $_GET['payamt_coins'],
		'paychannel' => $_GET['paychannel'],
		'paychannelsubid' => $_GET['paychannelsubid'],
		'payitem' => $_GET['payitem'],
		'ts' => $_GET['ts'],
		'providetype' => $_GET['providetype'],
		'pubacct_payamt_coins' => isset($_GET['pubacct_payamt_coins'])?$_GET['pubacct_payamt_coins']:'',//$_GET['pubacct_payamt_coins'],
		'token' => $_GET['token'],
		'version' => $_GET['version'],
		'zoneid' => $_GET['zoneid'],
	); */
$isright = SnsSigCheck::verifySig( $method, $url_path, $parr, $secret,$_GET['sig']);
$socket;
$port = 5400;
$host = '115.159.127.231';
if($isright == true){
	$socket = socket_create(AF_INET, SOCK_STREAM, SOL_TCP);
		if(!$socket){
			exit('创建socket失败');
		}
		$result = socket_connect($socket, $host, $port);
		if(!$result){
			exit('连接socket失败');
		}
		$result=socket_write($socket, pack('vva64a64a15a20a64a65a3a5a2a15a15a15a65',416,2,$_GET['openid'],$_GET['appid'],$_GET['ts'],$_GET['payitem'],$_GET['token'],$_GET['billno'],$_GET['version'],$_GET['zoneid'],$_GET['providetype'],$_GET['amt'],$_GET['payamt_coins'],$_GET['pubacct_payamt_coins'],$_GET['sig']));
		if(!$result){
			exit('发送失败');
		}
	$out = socket_read($socket, 5);
	$arr = unpack("vv1/vv2/cc1",$out);
	if ($arr['c1']==0) {
		print_r(json_encode(array(
				'ret' => '0',
				'msg' => 'OK',),true));
	} else {
		print_r(json_encode(array(
				'ret' => '4',
				'msg' => '请求参数错误：（sig）',),true));
	}

	socket_close($socket); 
} else {
	print_r(json_encode(array(
				'ret' => '4',
				'msg' => '请求参数错误：（sig）',),true));
}
function comfirm($isSuc)
{
	$len = 0;
	$isFinable = false;
	while(true){
		if($len>0){
			$sdk = new OpenApiV3($appid, $appkey);
			$sdk->setServerName('openapi.tencentyun.com');
			$ret = confirm_delivery($sdk,  $_GET['openid'], 'qzone',$isSuc);
			switch($ret['ret']){
				case '0':
				//成功
				$isFinable=true;
				//通知成功
				break;
				case '1001':
				//请求参数错误。
				$isFinable=true;
				break;
				case '1059':
				//TOKEN超时
				$isFinable=true;
				//通知错误
				break;
				case '1060':
				//订单已回滚。
				$isFinable=true;
				//通知错误
				break;
				case '1062':
				//通知过早订单尚未挂起，请稍候再试(收到发货回调后，2秒内应答，2秒后才调用此接口，可等待几秒后，重复通知)。
				//操作指引：可重试3次，还是挂起检查调用该接口时间，确认时间无误请求三次放弃此订单。
				break;
				case '1063':
				//订单不存在。
				$isFinable=true;
				break;
				case '1068':
				//交易已失败结束，通知过晚（未扣款）。
				$isFinable=true;
				break;
				case '1069':
				//交易已成功结束，通知过晚（已扣款）。
				$isFinable=true;
				break;
				case '1099':
				//系统繁忙，请稍候再试（可等待几秒后，重复通知）。
				break;
			}
		}
		if($isFinable==true){
			break;
		}
		if($len>=3){
			break;
		}
		sleep(10);
		$len++;
	}
}

function confirm_delivery($sdk, $openid, $pf, $isSuc)
{
	$params = array(
		'openid' => $openid,
		'pf' => $pf,
		'ts' => time(),
		'payitem' => $_GET['payitem'],
		'token_id' => $_GET['token'],
		'billno' => $_GET['billno'],
		'version' => $_GET['version'],
		'zoneid' => $_GET['zoneid'],
		'providetype' => $_GET['providetype'],
		'provide_errno' => $isSuc ? '0' : '4',
		'provide_errmsg' => $isSuc ? 'OK' : 'error',
		'amt' => isset($_GET['amt'])?$_GET['amt']:'0',
		'payamt_coins' =>isset($_GET['payamt_coins'])?$_GET['payamt_coins']:'0',
		'pubacct_payamt_coins' => isset($_GET['pubacct_payamt_coins'])?$_GET['pubacct_payamt_coins']:'0',
	);
	
	$script_name = '/v3/pay/confirm_delivery';
	return $sdk->api($script_name, $params,'post','https');
}
 /* if ($isright==true) {
		print_r(json_encode(array(
				'ret' => '0',
				'msg' => 'OK',),true));
	} else {
		print_r(json_encode(array(
				'ret' => '4',
				'msg' => '请求参数错误：（sig）',),true));
	} */
?>