﻿import smtplib, os
from email.MIMEMultipart import MIMEMultipart
from email.MIMEBase import MIMEBase
from email.MIMEText import MIMEText
from email.Utils import COMMASPACE, formatdate
from email import Encoders

def send_mail(send_from, send_to, subject, text, files=[], server="localhost"):
    assert type(send_to)==list
    assert type(files)==list

    msg = MIMEMultipart()
    msg['From'] = send_from
    msg['To'] = COMMASPACE.join(send_to)
    msg['Date'] = formatdate(localtime=True)
    msg['Subject'] = subject

    msg.attach( MIMEText(text) )

    for f in files:
        part = MIMEBase('application', "octet-stream")
        part.set_payload( open(f,"rb").read() )
        Encoders.encode_base64(part)
        part.add_header('Content-Disposition', 'attachment; filename="%s"' % os.path.basename(f))
        msg.attach(part)

    smtp = smtplib.SMTP('smtp.gmail.com:587')
	smtp.starttls()
    smtp.login('fu@gmail.com','fu')
    smtp.sendmail(send_from, send_to, msg.as_string())
    smtp.close()

ATTACHMENTS = ['/tmp/2013-11-04-test.csv']
send_from='fu@gmail.com'
send_to=['fu@gmail.com']
subject='adfadfadf'
text = 'adfadfadf'
send_mail(send_from, send_to, subject, text, files=ATTACHMENTS)