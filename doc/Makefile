ADOC_PDF=asciidoctor -r asciidoctor-pdf -r asciidoctor-diagram -b pdf
ADOC=asciidoctor -r asciidoctor-diagram

all: pdf html

pdf: vrviz_tech_doc_pdf vrviz_user_doc_pdf
	rm -r .asciidoctor

vrviz_tech_doc_pdf: vrviz_tech_doc.adoc
	$(ADOC_PDF) $< 

vrviz_user_doc_pdf: vrviz_user_doc.adoc
	$(ADOC_PDF) $<

html: vrviz_tech_doc_html vrviz_user_doc_html

vrviz_tech_doc_html: vrviz_tech_doc.adoc
	$(ADOC) $<

vrviz_user_doc_html: vrviz_user_doc.adoc
	$(ADOC) $<

clean:
	rm *.pdf *.html && rm -R .asciidoctor 
