import http from 'k6/http';
import { check } from 'k6';
import { Trend, Rate, Counter } from 'k6/metrics';

let getPartnerFilteredDuration = new Trend('get_partner_filtered_duration');
let getPartnerFilteredFailRate = new Rate('get_partner_filtered_fail_rate');
let getPartnerFilteredSuccessRate = new Rate('get_partner_filtered_success_rate');
let getPartnerFilteredRequests = new Counter('get_partner_filtered_requests');

const accessToken = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJQcm9qZXRvIENhcmFtZWwiLCJqdGkiOiIwM2NmMTA0YS05NmZkLTQ1MDUtYjUzNS1jYzQ4NjIwZjkwOTIiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjY2NmNhYTFiZWUyYmJkODM2OTlkZDVkNSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiZXhwIjoxNzIyNzE5MDkzLCJpc3MiOiJjYXJhbWVsLWFwaS1hdXRoIiwiYXVkIjoiY2FyYW1lbC1hcGktYXV0aCJ9.l7Wp60E1IRkBYG_R4QqOhfqACM_9aHF_kxAnz2rVpM8';

export default function () {
    const url = 'https://localhost:7127/users-control/partner/filtered';
    const params = {
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${accessToken}`
        },
    };

    const requestBody = JSON.stringify({
        partnerFilter: {
            partnerId: '666caa1bee2bbd83699dd5d5',
        },
        pagination: {
            page: 1,
            size: 10,
        },
    });

    let response = http.post(url, requestBody, params);

    console.log('GET Partner Filtered Response status: ', response.status);
    console.log('GET Partner Filtered Response body: ', response.body);

    getPartnerFilteredDuration.add(response.timings.duration);

    if (response.status >= 200 && response.status < 400) {
        getPartnerFilteredSuccessRate.add(1);
    } else {
        getPartnerFilteredFailRate.add(1);
    }

    getPartnerFilteredRequests.add(1);

    check(response, {
        'status is 200 (getPartnerFiltered)': (r) => r.status === 200,
        'response time is less than 4000ms (getPartnerFiltered)': (r) => r.timings.duration < 4000,
    });
}