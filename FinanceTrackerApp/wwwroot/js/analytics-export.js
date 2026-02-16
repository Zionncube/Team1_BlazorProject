window.financeAnalytics = {
    ensureJsPdf: async function () {
        if (window.jspdf && window.jspdf.jsPDF) {
            return;
        }

        const loadScript = (src) => new Promise((resolve, reject) => {
            const script = document.createElement('script');
            script.src = src;
            script.async = true;
            script.onload = resolve;
            script.onerror = reject;
            document.head.appendChild(script);
        });

        try {
            await loadScript('https://cdn.jsdelivr.net/npm/jspdf@2.5.1/dist/jspdf.umd.min.js');
        } catch {
            await loadScript('https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js');
        }

        if (!(window.jspdf && window.jspdf.jsPDF)) {
            throw new Error('jsPDF failed to load.');
        }
    },

    downloadTextFile: function (fileName, content, mimeType) {
        const blob = new Blob([content], { type: mimeType || 'text/plain;charset=utf-8' });
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = fileName;
        document.body.appendChild(a);
        a.click();
        a.remove();
        URL.revokeObjectURL(url);
    },

    downloadChartPng: async function (chartId, fileName) {
        if (!window.ApexCharts) {
            throw new Error('ApexCharts is not loaded.');
        }

        const result = await window.ApexCharts.exec(chartId, 'dataURI');
        if (!result || !result.imgURI) {
            throw new Error('Chart image export failed.');
        }

        const a = document.createElement('a');
        a.href = result.imgURI;
        a.download = fileName || `${chartId}.png`;
        document.body.appendChild(a);
        a.click();
        a.remove();
    },

    getChartPngDataUri: async function (chartId) {
        if (!window.ApexCharts) {
            throw new Error('ApexCharts is not loaded.');
        }
        try {
            const result = await window.ApexCharts.exec(chartId, 'dataURI');
            if (!result || !result.imgURI) {
                return '';
            }
            return result.imgURI;
        } catch {
            return '';
        }
    },

    openPrintView: function (htmlContent, title) {
        const w = window.open('', '_blank');
        if (!w) {
            throw new Error('Popup blocked');
        }

        w.document.open();
        w.document.write(htmlContent);
        w.document.title = title || 'Report';
        w.document.close();
        w.focus();
        setTimeout(() => {
            w.print();
        }, 300);
    },

    openPrintFrame: function (htmlContent) {
        const iframe = document.createElement('iframe');
        iframe.style.position = 'fixed';
        iframe.style.right = '0';
        iframe.style.bottom = '0';
        iframe.style.width = '0';
        iframe.style.height = '0';
        iframe.style.border = '0';
        document.body.appendChild(iframe);

        const doc = iframe.contentDocument || iframe.contentWindow.document;
        doc.open();
        doc.write(htmlContent);
        doc.close();

        setTimeout(() => {
            iframe.contentWindow.focus();
            iframe.contentWindow.print();
            setTimeout(() => iframe.remove(), 1500);
        }, 400);
    },

    downloadPdfReport: async function (report) {
        await this.ensureJsPdf();

        const pdf = new window.jspdf.jsPDF({
            orientation: 'portrait',
            unit: 'pt',
            format: 'a4'
        });

        const margin = 40;
        const pageWidth = pdf.internal.pageSize.getWidth();
        const contentWidth = pageWidth - margin * 2;
        let y = 42;

        const get = (obj, key, fallback = '') => {
            if (!obj) return fallback;
            if (obj[key] !== undefined && obj[key] !== null) return obj[key];
            const camel = key.charAt(0).toLowerCase() + key.slice(1);
            if (obj[camel] !== undefined && obj[camel] !== null) return obj[camel];
            return fallback;
        };

        pdf.setFont('helvetica', 'bold');
        pdf.setFontSize(20);
        pdf.text('Analytics Report', margin, y);
        y += 20;

        pdf.setFont('helvetica', 'normal');
        pdf.setFontSize(11);
        pdf.text(`Generated: ${get(report, 'GeneratedAt')}`, margin, y);
        y += 16;
        pdf.text(`Range: ${get(report, 'RangeLabel')}`, margin, y);
        y += 24;

        pdf.setFont('helvetica', 'bold');
        pdf.setFontSize(13);
        pdf.text('Summary', margin, y);
        y += 16;
        pdf.setFont('helvetica', 'normal');
        pdf.setFontSize(11);
        pdf.text(`Income: ${get(report, 'TotalIncome')}`, margin, y);
        y += 14;
        pdf.text(`Expenses: ${get(report, 'TotalExpense')}`, margin, y);
        y += 14;
        pdf.text(`Net: ${get(report, 'Net')}`, margin, y);
        y += 14;
        pdf.text(`Savings Rate: ${get(report, 'SavingsRate')}`, margin, y);
        y += 18;

        const addChart = (title, imageUri) => {
            if (!imageUri) return;
            if (y > 650) {
                pdf.addPage();
                y = 42;
            }
            pdf.setFont('helvetica', 'bold');
            pdf.setFontSize(12);
            pdf.text(title, margin, y);
            y += 8;
            const height = 190;
            pdf.addImage(imageUri, 'PNG', margin, y, contentWidth, height, undefined, 'FAST');
            y += height + 18;
        };

        addChart('Category Spend Chart', get(report, 'DonutImage'));
        addChart('Monthly Income vs Expense Chart', get(report, 'BarImage'));

        if (y > 620) {
            pdf.addPage();
            y = 42;
        }

        pdf.setFont('helvetica', 'bold');
        pdf.setFontSize(13);
        pdf.text('Top Expense Transactions', margin, y);
        y += 18;

        pdf.setFont('helvetica', 'normal');
        pdf.setFontSize(10);
        const headers = ['Date', 'Description', 'Category', 'Amount'];
        const colX = [margin, margin + 90, margin + 300, pageWidth - margin - 80];
        headers.forEach((h, i) => pdf.text(h, colX[i], y));
        y += 12;
        pdf.setLineWidth(0.5);
        pdf.line(margin, y, pageWidth - margin, y);
        y += 12;

        const rows = get(report, 'TopExpenses', []);
        rows.forEach((row) => {
            if (y > 780) {
                pdf.addPage();
                y = 42;
            }
            const date = String(get(row, 'Date', ''));
            const desc = String(get(row, 'Description', '')).slice(0, 36);
            const category = String(get(row, 'Category', '')).slice(0, 20);
            const amount = String(get(row, 'Amount', ''));
            pdf.text(date, colX[0], y);
            pdf.text(desc, colX[1], y);
            pdf.text(category, colX[2], y);
            pdf.text(amount, colX[3], y);
            y += 12;
        });

        pdf.save(get(report, 'FileName', 'analytics-report.pdf'));
    }
};
