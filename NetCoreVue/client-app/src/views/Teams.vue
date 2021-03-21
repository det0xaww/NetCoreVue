<template>
    <div class="about">
        <h1>NBA teams</h1>
        <router-link class="btn btn-success mb-3" to="/add">Add</router-link>
        <a href="https://localhost:44346/api/NBATeams/getfile" target="_blank" class="btn btn-md btn-primary ml-3 mb-3">Generate pdf file</a>
        <form method="get" @submit.prevent="getAll()" class="mb-3 ">
            <input type="text" autocomplete="off" placeholder="Type any search term" class="col-lg-3" v-model="searchTerm" />
            <input type="submit" class="btn btn-sm btn-success mb-1 ml-3 col-lg-2" value="Search" />
        </form>

        <div id="" class="d-flex justify-content-center">
            <div class="d-flex justify-content-center">
                <div class="table-responsive-md">
                    <table class="table table-bordered table-hover">
                        <thead class="">
                            <tr>
                                <td>Abbreviation</td>
                                <td>City</td>
                                <td>Conference</td>
                                <td>Division</td>
                                <td>Fullname</td>
                                <td>Name</td>
                                <td colspan="4">Options</td>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="record in records" :key="record.id">
                                <td>{{record.abbreviation}}</td>
                                <td>{{record.city}}</td>
                                <td>{{record.conference}}</td>
                                <td>{{record.division}}</td>
                                <td>{{record.full_name}}</td>
                                <td>{{record.name}}</td>
                                <td colspan="2"><router-link :to="{name:'Edit', params:{id: record.id}}" class="btn btn-sm btn-primary m-2">Edit</router-link></td>
                                <td colspan="2"><button class="btn btn-sm btn-danger m-2" @click="deleteNbaTeam(record.id)">Delete</button></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
</template>


<script>
    import api from '@/NBATeamsApiService';

  export default {
    data() {
        return {
        records: [],
        model: {},
        searchTerm: ''
      };
    },
    async created() {
      this.getAll()
    },
    methods: {
      async getAll() {
            this.records = await api.getAll(this.searchTerm)
      },

      async deleteNbaTeam(id) {
        if (confirm('Are you sure you want to delete this record?')) {
          // if we are editing a record we deleted, remove it from the form
          if (this.model.id === id) {
            this.model = {}
          }

          await api.delete(id)
          await this.getAll(this.searchTerm)
        }
      },

        async generateDocument() {
            console.log("create document test")
            await api.createDocument()
        }
    }
  }
</script>

<style scoped>
</style>
