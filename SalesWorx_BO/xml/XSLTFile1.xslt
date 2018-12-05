<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="xml" indent="yes"/>

    <xsl:template match="@* | node()">
      <Menus>
        <xsl:for-each select="Menu">
          <menu>
            <xsl:attribute name="id">
              <xsl:value-of select="id"/>
            </xsl:attribute>
            <xsl:attribute name="value">
              <xsl:value-of select="child::Title/text()"/>
            </xsl:attribute>
            <xsl:for-each select="Pages/Page">
              <submenu>
                <xsl:attribute name="id">
                  <xsl:value-of select="id"/>
                </xsl:attribute>
                <xsl:attribute name="value">
                  <xsl:value-of select="child::Title"/>
                </xsl:attribute>
              </submenu>
            </xsl:for-each>
          </menu>
        </xsl:for-each>
      </Menus>
    </xsl:template>
</xsl:stylesheet>
 
          
     