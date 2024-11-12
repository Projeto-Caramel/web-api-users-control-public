import http from 'k6/http';
import { check } from 'k6';
import { Trend, Rate, Counter } from 'k6/metrics';

let getPartnerByIdDuration = new Trend('get_partner_by_id_duration');
let getPartnerByIdFailRate = new Rate('get_partner_by_id_fail_rate');
let getPartnerByIdSuccessRate = new Rate('get_partner_by_id_success_rate');
let getPartnerByIdRequests = new Counter('get_partner_by_id_requests');

const accessToken = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJQcm9qZXRvIENhcmFtZWwiLCJqdGkiOiIwM2NmMTA0YS05NmZkLTQ1MDUtYjUzNS1jYzQ4NjIwZjkwOTIiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjY2NmNhYTFiZWUyYmJkODM2OTlkZDVkNSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiZXhwIjoxNzIyNzE5MDkzLCJpc3MiOiJjYXJhbWVsLWFwaS1hdXRoIiwiYXVkIjoiY2FyYW1lbC1hcGktYXV0aCJ9.l7Wp60E1IRkBYG_R4QqOhfqACM_9aHF_kxAnz2rVpM8';

export default function () {
    const url = 'https://localhost:7127/users-control/partner';
    const params = {
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${accessToken}`
        },
    };

    const partnerId = '666caa1bee2bbd83699dd5d5';

    let response = http.get(`${url}?id=${partnerId}`, params);

    console.log('GET Partner by ID Response status: ', response.status);
    console.log('GET Partner by ID Response body: ', response.body);

    getPartnerByIdDuration.add(response.timings.duration);

    if (response.status >= 200 && response.status < 400) {
        getPartnerByIdSuccessRate.add(1);
    } else {
        getPartnerByIdFailRate.add(1);
    }

    getPartnerByIdRequests.add(1);

    check(response, {
        'status is 200 (getPartnerById)': (r) => r.status === 200,
        'response time is less than 4000ms (getPartnerById)': (r) => r.timings.duration < 4000,
    });
}
