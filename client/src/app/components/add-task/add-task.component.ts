import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule} from '@angular/forms';
import {TaskService} from '../../services/task.service';
import {MatFormField, MatLabel} from '@angular/material/form-field';
import {MatInput} from '@angular/material/input';
import {MatButton} from '@angular/material/button';
import {Router} from '@angular/router';

@Component({
  selector: 'app-add-task',
  imports: [
    ReactiveFormsModule,
    MatInput,
    MatButton,
    MatLabel,
    MatFormField
  ],
  templateUrl: './add-task.component.html',
  styleUrl: './add-task.component.css'
})
export class AddTaskComponent implements OnInit {
  taskForm!: FormGroup;

  constructor(private fb: FormBuilder, private taskService: TaskService, private router: Router) { }

  ngOnInit(): void {
    this.taskForm = this.fb.group({
      name: [''],
      description: [''],
      assignedTo: ['']
    });
  }

  onSubmit(): void {
    if (this.taskForm.valid) {
      this.taskService.addTask(this.taskForm.value).subscribe((_ => this.router.navigate(['/'])));
    }
  }
}
