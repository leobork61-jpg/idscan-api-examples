# idscan-api-examples

Privacy-first identity verification using NFC passport verification and on-device biometric verification.

Features

- NFC passport verification
- ICAO 9303 compliant chip validation
- On-device selfie verification
- Identity verification API
- GDPR-friendly architecture
- Data minimization support

### Download the signed PDF report

After a verification is completed, the API can return a PDF report:

GET /scan-requests/{id}/report.pdf

The report is digitally sealed and includes a trusted timestamp. This allows the receiving system to store the report with the case file and later verify that the PDF has not been modified since it was issued.

Website:
https://www.id-scan.app

Documentation:
https://www.id-scan.app/en/integration
