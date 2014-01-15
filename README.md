NginxLogAnalyzer
================

This is an Nginx log analyzer which will parse access.log file of an Nginx server
and will store the result into ".csv" file.
Result contains following fields from access.log file.
1 : Request count 
2 : Maximum Request Time.
3 : Minimum Request Time.
4 : 200 Status Count.
5 : 500 Status Count.
6 : Method Type.

Note : Please open output "*.csv" file with tab('\t') as a delimiter.

example : 
Log Format used to generate access.log file:
log_format main '$remote_addr - $remote_user [$time_local] '
                '"$request" $status $body_bytes_sent [$request_time]'
                '"$http_referer" "$http_user_agent"'; 

Sample Log file content : 
123.45.678.90 - - [13/Jan/2014:09:22:01 +0000] "GET /xyz/abc/pqr/sample HTTP/1.1" 200 342 [0.063] "-" "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:26.0) Gecko/20100101 Firefox/26.0"
123.45.678.90 - - [13/Jan/2014:09:23:01 +0000] "GET /xyz/abc/pqr/sample HTTP/1.1" 500 123 [0.100] "-" "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:26.0) Gecko/20100101 Firefox/26.0"
....

Sample Output file content : 
RequestCount	MethodType	StatusCount200	StatusCount500	Maximum Request Time	Minimum Request Time	Request Name																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																									
	1			      	GET		        	1			        	1				        	0.100				        	0.063		    	/xyz/abc/pqr/sample
  
