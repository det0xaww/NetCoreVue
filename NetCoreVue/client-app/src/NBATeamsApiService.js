import Vue from 'vue'
import axios from 'axios'


const client = axios.create({
    baseURL: 'https://localhost:44346/api/NBATeams',
    json: true,
})


export default {
    async execute(method, resource, data) {
        const accessToken = await Vue.prototype.$auth.getAccessToken()
        return client({
            method,
            url: resource,
            data,
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
                'Access-Control-Allow-Origin': '*',
                'Authorization': `Bearer ${accessToken}`
            }

        }).then(req => {

            if (Object.prototype.toString.call(req.data) !== '[object Array]') {
                Notification(req.data.group, req.data.type, req.data.title, req.data.text);
            }

            return req.data
        }).catch(error => {
            console.log("TEST:" + error.response);
            if (!error.response) {
                // network error
                this.errorStatus = 'Error: Network Error';
            } else {
                this.errorStatus = error.response.data.message;
            }
        })
    },
    getAll(searchTerm) {
        return this.execute('get', `/getteams/${searchTerm}`)
    },
    create(data) {
        return this.execute('post', '/add/', data)
    },
    updateGet(id) {
        return this.execute('get', `/edit/${id}`)
    },
    updatePut(id, data) {
        return this.execute('put', `/edit/${id}`, data)
    },
    delete(id) {
        return this.execute('post', `/delete/${id}`)
    },
    createDocument() {
        return this.execute('get', '/getfile')
    }

}


function Notification(group, type, title, text) {
    Vue.notify({
        group: group,

        type: type,

        title: title,

        text: text,
    })
}
