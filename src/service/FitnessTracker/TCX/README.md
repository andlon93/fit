# TCX

Skjemaet som definerer Garmin sitt Training Center filformat ligger [her](https://www8.garmin.com/xmlschemas/TrainingCenterDatabasev2.xsd).

C#-klassene er generert ved å kjøre
```
xsd .\TrainingCenterDatabaseV2.xsd /c /language:CS
```
Se https://docs.microsoft.com/en-us/dotnet/standard/serialization/xml-schema-def-tool-gen

Initialiseringen av `new XmlSerializer(typeof(TrainingCenterDatabase_t))` feilet nå med
```
System.PlatformNotSupportedException: Compiling JScript/CSharp scripts is not supported
```

Heldigvis har [noen](https://justsimplycode.com/2018/12/29/badly-auto-generated-wcf-proxy/) vært borti dette problemet før og det forsvant ved å endre alle todimensjonale arrays til vanlige arrays, fra `[][]` til `[]` (nevner også at `XElement[] Any` må fjernes, men det var ingen slike).