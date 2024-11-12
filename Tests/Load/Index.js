import getPartners from "./Scenarios/Partners/get-partners.js";
import getPartnerById from "./Scenarios/Partners/get-partner-by-id.js";
import getPartnerByEmail from "./Scenarios/Partners/get-partner-by-email.js";
import getPartnerFiltered from "./Scenarios/Partners/get-partner-filtered.js";
import { group, sleep } from 'k6';
import { htmlReport } from "https://raw.githubusercontent.com/benc-uk/k6-reporter/2.2.0/dist/bundle.js";
import { textSummary } from "https://jslib.k6.io/k6-summary/0.0.1/index.js";

export let options = {
    stages: [
        { duration: '30s', target: 10 }, // Ramp-up para 10 usuários durante 30 segundos
        { duration: '1m', target: 10 },  // Mantém 10 usuários por 1 minuto
        { duration: '30s', target: 0 },  // Ramp-down para 0 usuários durante 30 segundos
    ],
    thresholds: {
        'get_partners_duration': ['p(95)<4000'], // 95% das requisições devem ser completadas em menos de 4000ms
        'get_partners_fail_rate': ['rate<0.05'], // Taxa de falhas deve ser menor que 5%
        'get_partner_by_id_duration': ['p(95)<4000'],
        'get_partner_by_id_fail_rate': ['rate<0.05'],
        'get_partner_by_email_duration': ['p(95)<4000'],
        'get_partner_by_email_fail_rate': ['rate<0.05'],
        'get_partner_filtered_duration': ['p(95)<4000'],
        'get_partner_filtered_fail_rate': ['rate<0.05'],
    },
};

export default function () {
    group('Load Test for Get Partners Endpoint', () => {
        getPartners();
    });

    group('Load Test for Get Partner by ID Endpoint', () => {
        getPartnerById();
    });

    group('Load Test for Get Partner by Email Endpoint', () => {
        getPartnerByEmail();
    });

    group('Load Test for Get Partner Filtered Endpoint', () => {
        getPartnerFiltered();
    });

    sleep(1);
}

// Função para gerar o relatório HTML e JSON
export function handleSummary(data) {
    return {
        "summary.html": htmlReport(data),
        "summary.json": JSON.stringify(data, null, 2),
        stdout: textSummary(data, { indent: " ", enableColors: true }),
    };
}
